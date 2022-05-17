namespace FireSaverApi.Common
{
    public class RouteBuilderHelper
    {
        public static ImagePoint GetImagePoint(int x, int y, ImagePointArray availablePath)
        {
            if (y < 0)
                return null;
            else if (y >= availablePath.GetLength(0))
                return null;
            else if (x < 0)
                return null;
            else if (x >= availablePath.GetLength(1))
                return null;
            else
                return availablePath[y, x];
        }
    }
}