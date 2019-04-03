using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Returninator.Gameplay
{
    public enum GameState
    {
        None,
        Starting,
        Playing,
        Finishing,
        Waiting,
    }

    public class TeamCharacters: IReadOnlyList<Character>
    {
        private List<Character> Characters { get; } = new List<Character>();
        
        public Character this[int index] => Characters[index];
        public int Count => Characters.Count;
        public IEnumerator<Character> GetEnumerator() => Characters.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Characters.GetEnumerator();
    }

    public class SessionManager: MonoBehaviour
    {
        public static SessionManager Instance { get; private set; }
        public static bool HasInstance => Instance != null;

        public GameState CurrentState { get; private set; } = GameState.None;

        public Dictionary<int, TeamCharacters> Teams { get; } = new Dictionary<int, TeamCharacters>();

        private void OnEnable()
        {
            if (HasInstance)
            {
                Debug.LogError("MatchManager instance already exists!");
                Destroy(this);
                return;
            }

            Instance = this;
        }
        
        private void OnDisable()
        {
            if (Instance != this)
                return;
            
            Instance = null;


        }
    }
}