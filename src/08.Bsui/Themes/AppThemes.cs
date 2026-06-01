using MudBlazor;

namespace Obscura.FinanceTracker.Bsui.Themes
{
    // ============================================================
    // AppThemes.cs
    // Contains all static theme configurations for the application.
    // ============================================================
    public static class AppThemes
    {
        // ========================================================
        // THEME 1: OBSIDIAN (Retro Dark & Purple Accent)
        // ========================================================
        public static MudTheme ObsidianTheme { get; } = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Background = "#0E0E10",
                Surface = "#17171A",
                DrawerBackground = "#17171A",
                AppbarBackground = "#17171A",
                TextPrimary = "#EBEBEB",
                TextSecondary = "#909098",
                Primary = "#6366F1",
                PrimaryContrastText = "#FFFFFF",
                Success = "#5CC87A",
                Error = "#E05252",
                Warning = "#D4A44C",
                LinesDefault = "#2C2C32",
                LinesInputs = "#3A3A42",
                HoverOpacity = 0.08,
                ActionDisabledBackground = "#202024"
            },
            PaletteLight = new PaletteLight()
            {
                Primary = "#6366F1",
                AppbarBackground = "#6366F1"
            },
            Typography = new Typography(),
            LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "4px" }
        };

        // ========================================================
        // THEME 2: COBALT (True Black & Monospace)
        // ========================================================
        public static MudTheme CobaltTheme { get; } = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Background = "#000000",
                Surface = "#0A0A0A",
                DrawerBackground = "#000000",
                AppbarBackground = "#000000",
                TextPrimary = "#F4F4F5",
                TextSecondary = "#A1A1AA",
                Primary = "#FFFFFF",
                PrimaryContrastText = "#000000",
                Success = "#10B981",
                Error = "#EF4444",
                Warning = "#F59E0B",
                LinesDefault = "#27272A",
                LinesInputs = "#27272A",
                HoverOpacity = 0.1
            },
            PaletteLight = new PaletteLight()
            {
                Primary = "#000000",
                AppbarBackground = "#000000"
            },
            Typography = new Typography()
            {
                Default = {
                    FontFamily = ["JetBrains Mono", "monospace"]
                },
                H5 = { 
                    FontFamily = ["JetBrains Mono", "monospace"],
                    //TextTransform = "lowercase",
                    FontWeight = "700" 
                },
                H6 = {
                    FontFamily = ["JetBrains Mono", "monospace"],
                    //TextTransform = "lowercase",
                    FontWeight = "700"
                },
                Button = {
                    FontFamily = ["JetBrains Mono", "monospace"],
                    //TextTransform = "lowercase",
                    FontWeight = "500"
                }
            },
            LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "12px" }
        };

        // ========================================================
        // THEME 3: DRACULA (Dark Blue/Gray & Pink Accent)
        // ========================================================
        public static MudTheme DraculaTheme { get; } = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Background = "#282A36",
                Surface = "#44475A",
                DrawerBackground = "#282A36",
                AppbarBackground = "#1E1F29",
                TextPrimary = "#F8F8F2",
                TextSecondary = "#6272A4",
                Primary = "#FF79C6",
                PrimaryContrastText = "#282A36",
                Success = "#50FA7B",
                Error = "#FF5555",
                Warning = "#F1FA8C",
                LinesDefault = "#6272A4",
                LinesInputs = "#6272A4",
                HoverOpacity = 0.15
            },
            PaletteLight = new PaletteLight()
            {
                Primary = "#FF79C6",
                AppbarBackground = "#282A36"
            },
            Typography = new Typography(),
            LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "8px" }
        };
    }
}