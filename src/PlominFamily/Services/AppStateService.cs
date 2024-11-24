using Utxorpc.Sdk;
using Utxorpc.Sdk.Models;
using Utxorpc.V1alpha.Query;

namespace PlominFamily.Services;

public class AppStateService
{
    public event Action? OnBalanceUpdate;
    public ulong Balance { get; private set; }

    private QueryServiceClient QueryServiceClient { get; }
    private SyncServiceClient SyncServiceClient { get; }

    public AppStateService(QueryServiceClient queryServiceClient, SyncServiceClient syncServiceClient)
    {
        QueryServiceClient = queryServiceClient;
        SyncServiceClient = syncServiceClient;

        _ = Task.Run(async () =>
        {
            await BalanceUpdateAsync();
            await foreach (NextResponse? _ in SyncServiceClient.FollowTipAsync())
            {
                await BalanceUpdateAsync();
            }
        });
    }

    private async Task BalanceUpdateAsync()
    {
        var utxos = new List<AnyUtxoData>();
        var address = Convert.FromHexString("0147b6c7f2df5c7f920e5819e3a69c287d21fb9bbd7dec28974176d544b5f1dbaee027a5a74b4c7d18b4773e9dc3aa9ef6435291ae22d9e0d1");
        string? nextToken = null;

        do
        {
            var result = await QueryServiceClient.SearchUtxosAsync(address, nextToken);
            utxos.AddRange(result.Items);
            nextToken = result.NextToken;
        } while (!string.IsNullOrEmpty(nextToken));
        Balance = utxos.Aggregate(0UL, (acc, utxo) => acc + utxo.Cardano.Coin);
        OnBalanceUpdate?.Invoke();
    }
}