namespace Unimake.GenerateAccessToken.Models
{
    internal struct ObjectResponseResult<T>
    {
        #region Public Properties

        public T Object { get; }

        public string Text { get; }

        #endregion Public Properties

        #region Public Constructors

        public ObjectResponseResult(T responseObject, string responseText)
        {
            Object = responseObject;
            Text = responseText;
        }

        #endregion Public Constructors
    }
}