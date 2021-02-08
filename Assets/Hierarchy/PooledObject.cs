namespace Assets
{
    /// <summary>
    /// Represents an object that can be stored inside of an Object Pool.
    /// </summary>
    /// <inheritdoc/>
    public abstract class PooledObject : ManagedVelocityObject
    {
        /// <summary>
        /// An ID assigned to this object by its Object Pool when it's spawned.
        /// Guaranteed to be unique from any other active object spawned from the same pool.
        /// </summary>
        public int SpawnId;

        /// <summary>
        /// The name initially given to this object by Unity.
        /// </summary>
        public string InitialName { get; set; }

        /// <summary>
        /// The name that will be assigned to this object when it's spawned.
        /// </summary>
        public string SpawnName => $"{InitialName} {SpawnId}";

        /// <summary>
        /// Subclass-specific functionality to happen when an object is activated.
        /// </summary>
        protected virtual void OnActivate() { }

        /// <summary>
        /// Activates this game object, and calls any subclass-specific implementation of OnActivate().
        /// </summary>
        public void ActivateSelf()
        {
            gameObject.SetActive(true);
            name = SpawnName;
            OnActivate();
        }

        /// <summary>
        /// Subclass-specific functionality to happen when an object is deactivated.
        /// </summary>
        protected virtual void OnDeactivate() { }

        /// <summary>
        /// Activates this game object, and calls any subclass-specific implementation of OnDeactivate()
        /// </summary>
        public void DeactivateSelf()
        {
            // Setting an object to inactive may cause it to call the OnTriggerExit method
            // with the Destructor, which could cause this method to be called twice in a row.
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                OnDeactivate();
            }
        }

        /// <summary>
        /// Calls GameManager.CreateFleetingText() using the given <paramref name="text"/>
        /// at the center of this object.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void CreateFleetingTextAtCenter(object text)
        {
            GameManager.Instance.CreateFleetingText(text.ToString(), this.transform.position);
        }
    }
}
