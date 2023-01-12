using Godot;
using System;

public static class AnimationPlayerExtension
{
	public static void PlayOrQueueAnimation(this AnimationPlayer animationPlayer, string animationName, bool shallQueue, bool clearQueueInTheBeggining) {
		if(clearQueueInTheBeggining) {
			animationPlayer.ClearQueue();
		}
		if(shallQueue) {
			animationPlayer.Queue(animationName);
		}
		else {
			animationPlayer.Play(animationName);
		}
	}
}
