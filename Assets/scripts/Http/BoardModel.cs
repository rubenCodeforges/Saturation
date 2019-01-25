using System;

namespace Http
{
    [Serializable]
    public class BoardModel
    {
        private string id;
        public string gameName;
        public string userMachineId;
        public string userName;
        public string score;
    }
}