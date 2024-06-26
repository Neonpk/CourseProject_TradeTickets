using System.Text.RegularExpressions;

namespace CourseProject_SellingTickets.Extensions;

public static class StringExtensions
{

    public static bool Like(this string toSearch, string toFind) 
    {
        return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").
                Replace(toFind, ch => @"\" + ch).
                    Replace('_', '.').
                        Replace("%", ".*") + @"\z", RegexOptions.Singleline).
                            IsMatch(toSearch);
    }

}