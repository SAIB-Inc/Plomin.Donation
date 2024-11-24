module.exports = {
  important: true,
  content: [
    "../Components/**/*.{razor,html,cs}", // All Razor components and C# files in the Components folder
    "../Program.cs",                      // Program.cs if it contains Tailwind utilities
    "./**/*.html",                        // HTML files within wwwroot
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};