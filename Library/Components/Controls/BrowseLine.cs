using Godot;
using Godot.Sharp.Extras;
using NativeFileDialogs.Net;

// namespace

namespace GodotManager.Library.Components.Controls;

[Tool]
public partial class BrowseLine : Control
{
	#region Signals
	#endregion
	
	#region Quick Create
	public static BrowseLine FromScene()
	{
		var scene = GD.Load<PackedScene>("res://Library/Components/Controls/BrowseLine.tscn");
		return scene.Instantiate<BrowseLine>();
	}
	#endregion
	
	#region Node Paths

	[NodePath] private Label _textLabel;
	[NodePath] private LineEdit _input;
	[NodePath] private Button _browse;
	[NodePath] private Button _default;
	#endregion
	
	#region Private Variables

	private string _displayText;
	private string _defaultValue;
	private int _textWidth;
	#endregion
	
	#region Public Variables

	[Export]
	public string DisplayText
	{
		get => _displayText;
		set
		{
			_displayText = value;
			if (_textLabel != null)
				_textLabel.Text = value;
		}
	}

	[Export]
	public string DefaultValue
	{
		get => _defaultValue;
		set
		{
			_defaultValue = value;
			if (_input != null)
				_input.Text = value;
			if (_default != null)
				_default.Visible = !string.IsNullOrEmpty(value);
		}
	}

	[Export]
	public int LabelWidth
	{
		get => _textWidth;
		set
		{
			_textWidth = value;
			if (_textLabel != null)
				_textLabel.CustomMinimumSize = new Vector2I(value, 25);
		}
	}

	public string Result => _input.Text;
	#endregion

	#region Godot Overrides
	public override void _Ready()
	{
		this.OnReady();
		
		// Rest of Initialization Functions
		DisplayText = _displayText;
		DefaultValue = _defaultValue;
		LabelWidth = _textWidth;

		_browse.Pressed += () =>
		{
			var path = "";
			var result = Nfd.PickFolder(out path);
			if (result == NfdStatus.Ok)
				_input.Text = path;
		};

		_default.Pressed += () => _input.Text = DefaultValue;
	}
	#endregion
	
	#region Event Handlers
	#endregion
	
	#region Private Support Functions
	#endregion
	
	#region Public Support Functions
	#endregion
}