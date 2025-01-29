using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BuSHA_CSEdition.Converters;

public class ImageConverter: IValueConverter
{
    private readonly Bitmap _checkImage = new(AssetLoader.Open(new Uri("avares://BuSHA_CSEdition/Assets/check_circle.png")));
    private readonly Bitmap _pendingImage = new(AssetLoader.Open(new Uri("avares://BuSHA_CSEdition/Assets/pending.png")));
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isSaved = (Boolean) (value ?? false);
        var image = isSaved ? _checkImage : _pendingImage;
        return image;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}