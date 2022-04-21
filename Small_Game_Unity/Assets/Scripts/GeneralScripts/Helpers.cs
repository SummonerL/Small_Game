using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    /**
    * Stole from:
    * https://stackoverflow.com/questions/22368434/best-way-to-split-string-into-lines-with-maximum-length-without-breaking-words
    *
    * This is a super helpful function which splits a string into a collection of strings, based on the maximum line length.
    * This function also ensures that no word is split. The only exception that would break this would be a super long word > 
    * the maximum length. In which case, the no-splitting rule would be prioritized, and keep the word on the same line
    *
     **/
    public static MatchCollection SplitToLines(string stringToSplit, int maximumLineLength)
    {
        return Regex.Matches(stringToSplit, @"(.{1," + maximumLineLength +@"})(?:\s|$)");
    }

    public static string GetTagValue(string key, List<string> currentTags) {
        string fullString = String.Join(",", currentTags);

        Match tagValue = Regex.Match(fullString, "(?<="+ key +":)(.*?)(?=\\,|$)");

        return tagValue.ToString();
    }
}
