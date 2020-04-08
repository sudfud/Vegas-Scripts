using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using Microsoft.VisualBasic;

using Sony.Vegas;

public class EntryPoint {
	public void FromVegas(Vegas vegas) {
		Project proj = vegas.Project;
		
		TrackEvent[] clips = FindSelectedEvents(proj);
		
		int frameOffset = 0;
		
		string input = Interaction.InputBox("Frame offset", "Chop", "1", -1, -1);

		if (input == "")
			return;
		
		if (int.TryParse(input, out frameOffset)) {
			foreach (TrackEvent clip in clips)
				Chop(clip, Timecode.FromFrames(frameOffset));
		}
		else
			MessageBox.Show("Please enter a number!");
	}

	private Track FindSelectedTrack(Project project) {
		foreach (Track track in project.Tracks) {
			if (track.Selected) {
				return track;
			}
		}
		return null;
	}

	private TrackEvent[] FindSelectedEvents(Project project) {
		List<TrackEvent> events = new List<TrackEvent>();
		foreach (Track track in project.Tracks) {
			foreach (TrackEvent clip in track.Events) {
				if (clip.Selected) {
					events.Add(clip);
				}
			}
		}
		return events.ToArray();
	}
	
	private void Chop(TrackEvent clip, Timecode offset) {
		while (clip.Length.FrameCount > 1) {
			clip = clip.Split(offset);
		}
	}
}
