using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* 	Simple Event System. Receives events and sends it to it's listeners
	Can be called from everywhere like this:

	EventManager.AddListener(Callback);

	// According function
	void Callback(string e) {
		Debug.Log(e);
	}


	Events can be globally triggered:
	EventManager.TriggerEvent("eventname");
*/
namespace TutorialDesigner
{
	/// <summary>
	/// Simple Event System. Receives events and sends it to it's listeners
	/// </summary>
	public static class EventManager{

		/// <summary>
		/// callback funktion that will be the listener.
		/// </summary>
		public delegate void EventCall(string e);
		static private List<EventCall> EventListeners;

		/// <summary>
		/// Initialization, creates a List of EventListeners 
		/// </summary>
		public static void Initialize() {
			EventListeners = new List<EventCall>();
		}

		/// <summary>
		/// Adds a new listener to EventListeners
		/// </summary>
		/// <param name="ec">New event call</param>
		public static void AddListener(EventCall ec) {
			if (EventListeners != null) {
				EventListeners.Add(ec);
			} else {
				Debug.LogError("EventManager was not initialized");
			}
		}

		/// <summary>
		/// Searches EventListeners for this event and executes it
		/// </summary>
		/// <param name="e">Event to be executed</param>
		public static void TriggerEvent(string e) {
			if (EventListeners != null) {
				for (int i=0; i<EventListeners.Count; i++) {
					EventListeners[i](e);
				}
			}
		}

		/// <summary>
		/// Removes a listener from EventListeners
		/// </summary>
		/// <param name="ec">Event call to be removed</param>
		public static void RemoveListener(EventCall ec) {
			EventListeners.Remove (ec);
		}
	}
}
