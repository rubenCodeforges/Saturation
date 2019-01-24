using System.Collections;
using UnityEngine.Networking;

namespace Http
{
    public static class HttpService
    {
        public static IEnumerator Get(string url)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            yield return uwr.SendWebRequest();
        }
    }
}