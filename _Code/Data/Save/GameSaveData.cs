using System;

namespace Data.Save
{
    [Serializable]
    public class GameSaveData
    {
        public string Nickname;
        public int SoftCurrency;
        
        public bool VibroDisabled;
        public bool AudioDisabled;
    }
}