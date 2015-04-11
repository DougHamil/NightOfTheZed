using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SM {

	public delegate void StateChangedDelegate();
	public delegate string TransitionDelegate(Dictionary<string, object> bindings);

	public class StateMachine {

		private Dictionary<string, object> bindings;
		private Dictionary<string, List<TransitionDelegate>> transitionsByState;
		private Dictionary<string, StateChangedDelegate> stateChangeDelegates;
		private string currentState;

		public StateMachine(string initialState) {
			bindings = new Dictionary<string, object>();
			transitionsByState = new Dictionary<string, List<TransitionDelegate>>();
			stateChangeDelegates = new Dictionary<string, StateChangedDelegate>();
			currentState = initialState;
		}

		public void AddCallback(string state, StateChangedDelegate dele)
		{
			stateChangeDelegates[state] = dele;
		}

		public void AddTransition(string state, TransitionDelegate tran)
		{
			if(!transitionsByState.ContainsKey(state))
				transitionsByState.Add(state, new List<TransitionDelegate>());
			transitionsByState[state].Add(tran);
		}

		public void SetParameter(string name, object value)
		{
			bindings[name] = value;
		}

		public void Update() {
			List<TransitionDelegate> transitions = transitionsByState[currentState];
			if(transitions != null)
			{
				foreach(TransitionDelegate tran in transitions)
				{
					string newState = tran(bindings);
					if(newState != null)
					{
						moveToState(newState);
						break;
					}
				}
			}
		}

		private void moveToState(string newState)
		{
			this.currentState = newState;
			StateChangedDelegate dele = this.stateChangeDelegates[newState];
			if(dele != null)
				dele();
		}
	}
}
