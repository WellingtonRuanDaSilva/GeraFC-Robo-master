namespace VASS06_GeraFC_Robo
{
    public static class Utils
    {
        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            return pos < 0 ? text : text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
