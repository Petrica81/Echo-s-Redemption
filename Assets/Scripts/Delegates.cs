
using System.Collections;

public static class Delegates
{
    public delegate void PlayAction();
    public delegate IEnumerator PlayActionCoro();
    public delegate void PlayActionOneArg(string text);
}
