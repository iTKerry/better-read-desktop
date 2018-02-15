using System.Collections.Generic;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace LoveRead.Infrastructure.Services
{
    public class PaletteService : IPaletteService
    {
        public IEnumerable<Swatch> Swatches { get; }

        public PaletteService()
            => Swatches = new SwatchesProvider().Swatches;

        public void ApplyBase(bool isDark)
            => new PaletteHelper().SetLightDark(isDark);

        public void ApplyPrimary(Swatch swatch)
            => new PaletteHelper().ReplacePrimaryColor(swatch);

        public void ApplyAccent(Swatch swatch)
            => new PaletteHelper().ReplaceAccentColor(swatch);
    }

    public interface IPaletteService
    {
        void ApplyBase(bool isDark);
        void ApplyPrimary(Swatch swatch);
        void ApplyAccent(Swatch swatch);
    }
}
