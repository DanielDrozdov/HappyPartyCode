using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UniRx;
using UnityEngine;

namespace Data.Save
{
    public class GameDataSaver : IGameDataAccess
    {
        #region [Properties]

        public GameSaveData GameSaveData => _data;
        
        private string FullPath
        {
            get
            {
                if (_fullPath != null) return _fullPath;

                string path = Path.Combine(DirPath, FileName);

                _fullPath = path;

                return _fullPath;
            }
        }
        
        private string DirPath => _dirPath ??= Path.Combine(Application.persistentDataPath, DirName);

        private string _fullPath;
        private string _dirPath;

        private const string DirName = "Data";
        private const string FileName = "PlayerData.dat";

        #endregion

        #region [Fields -- Generic]
        
        private bool SavedThisFrame => Time.frameCount == _lastSavedFrame;
        private int _lastSavedFrame = -1;

        private GameSaveData _data;
        private readonly BinaryFormatter Bf = new BinaryFormatter();

        #endregion
        
        public GameDataSaver()
        {
            LoadData();

            SubscribeSaveDataOnApplicationFocus();
            SubscribeSaveDataOnApplicationPause();
        }

        private void SubscribeSaveDataOnApplicationFocus()
        {
            Observable
                .EveryApplicationFocus()
                .Subscribe(isFocused =>
                {
                    if (!isFocused) SaveData();
                });
        }

        private void SubscribeSaveDataOnApplicationPause()
        {
            Observable
                .EveryApplicationPause()
                .Subscribe(isPaused =>
                {
                    if (isPaused) SaveData();
                });
        }

        #region [Save / Load]

        private void LoadData()
        {
            if (!File.Exists(FullPath))
            {
                _data = new GameSaveData();
                return;
            }

            FileStream fs = null;
            
            try
            {
                fs = new FileStream(FullPath, FileMode.Open);
                _data = (GameSaveData)Bf.Deserialize(fs);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("PlayerProfile:: Failed to load player data (" + ex.Message + "). Using new instance");
            }
            finally
            {
                _data ??= new GameSaveData();

                try
                {
                    fs?.Close();
                }
                catch (Exception ex)
                {
                    // Ignore IO exceptions
                    Debug.LogWarning("PlayerProfile:: Failed to load data stream. Reason: " + ex.Message);
                }
            }
        }

        private void SaveData()
        {
            if (SavedThisFrame) return;

            Directory.CreateDirectory(DirPath);
            FileStream fs = null;
            
            try
            {
                fs = new FileStream(FullPath, FileMode.Create);
                Bf.Serialize(fs, _data);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("PlayerProfile:: Failed to save player data: " + ex.Message);
            }
            finally
            {
                try
                {
                    fs?.Close();
                }
                catch (Exception ex)
                {
                    // Ignore IO exceptions
                    Debug.LogWarning("PlayerProfile:: Failed to save data. Reason: " + ex.Message);
                }
            }

            _lastSavedFrame = Time.frameCount;
        }

        #endregion
    }
}