using UnityEngine;

namespace Data.Levels
{
    [CreateAssetMenu(menuName = "Data/Levels/Create Mini Games List", fileName = "MiniGamesListData")]
    public class MiniGamesListData : ScriptableObject
    {
        [field: SerializeField] 
        public LevelSettingsData[] MiniGames { get; private set; }
    }
}
