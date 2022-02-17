namespace Abstract.EnvironmentVariables
{
    /// <summary>
    /// Allows us to store data and get it depending on if the game is in the editor, stable or development build.
    /// </summary>
    [System.Serializable]
    public struct EnvironmentVariable
    {
        public string name;
        public string stable;
        public string devBuild;
        public string editor;
        
        /// <summary>
        /// Gets the data for the correct version (Editor/Development Build/Release)
        /// </summary>
        /// <returns>The data for the version</returns>
        public string GetData()
        {
#if UNITY_EDITOR
            return editor;
#elif DEVELOPMENT_BUILD
            return devBuild;
#else
            return stable;
#endif
        }
        
        /// <summary>
        /// Sets the data for the current version
        /// </summary>
        public void SetData()
        {
#if UNITY_EDITOR
            System.Environment.SetEnvironmentVariable(name, editor);
#elif DEVELOPMENT_BUILD
            System.Environment.SetEnvironmentVariable(name, devBuild);
#else
            System.Environment.SetEnvironmentVariable(name, stable);
#endif
        }
    }
}
