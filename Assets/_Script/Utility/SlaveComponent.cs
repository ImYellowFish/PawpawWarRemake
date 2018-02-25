using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImYellowFish.Utility
{

    /// <summary>
    /// A Slave MonoBehaviour which is owned by a master MonoBehaviour.
    /// Provides Init() and CleanUp() so that you can control the order of initialization of many components.
    /// </summary>
    public class SlaveComponent<THost> : MonoBehaviour where THost : MonoBehaviour
    {
        protected THost host;

        public virtual void Init(THost host)
        {
            this.host = host;
        }

        public virtual void CleanUp()
        {

        }
    }

    /// <summary>
    /// A MonoBehaviour which contains a list of slave MonoBehaviours.
    /// For better control over many components.
    /// </summary>
    public class SlaveComponentContainer<THost, TSlave>
        where THost : MonoBehaviour
        where TSlave : SlaveComponent<THost>
    {
        protected List<TSlave> m_slaveComponents = new List<TSlave>();

        public IList<TSlave> Slaves { get { return m_slaveComponents; } }

        /// <summary>
        /// Create and initialize a new component
        /// </summary>
        public T CreateSlaveComponent<T>(THost host) where T : TSlave
        {
            return CreateSlaveComponent<T>(host, host.gameObject);
        }

        /// <summary>
        /// Create and initialize a new component
        /// </summary>
        public T CreateSlaveComponent<T>(THost host, GameObject slaveGameObject) where T : TSlave
        {
            var cpt = slaveGameObject.AddComponent<T>();
            cpt.Init(host);
            m_slaveComponents.Add(cpt);
            return cpt;
        }

        /// <summary>
        /// Find and initialize a component
        /// </summary>
        public T AddExistingSlaveComponent<T>(THost host) where T : TSlave
        {
            return AddExistingSlaveComponent<T>(host, host.gameObject);
        }

        /// <summary>
        /// Find and initialize a component
        /// </summary>
        public T AddExistingSlaveComponent<T>(THost host, GameObject slaveGameObject) where T : TSlave
        {
            var cpt = slaveGameObject.GetComponent<T>();
            cpt.Init(host);
            m_slaveComponents.Add(cpt);
            return cpt;
        }

        /// <summary>
        /// Find and initialize all components of a type
        /// </summary>
        public T[] AddExistingSlaveComponents<T>(THost host, GameObject slaveGameObject) where T : TSlave
        {
            var cpts = slaveGameObject.GetComponents<T>();

            foreach(var cpt in cpts)
            {
                cpt.Init(host);
                m_slaveComponents.Add(cpt);
            }
            return cpts;
        }

        /// <summary>
        /// Remove an existing slave component
        /// </summary>
        public void RemoveSlaveComponent(TSlave slave)
        {
            slave.CleanUp();
            m_slaveComponents.Remove(slave);
            Object.Destroy(slave);
        }
    }

}