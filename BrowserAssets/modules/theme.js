function SetTheme() {

  Blockly.Themes.Kiberlab = Blockly.Theme.defineTheme('kiberlab', {
    'base': Blockly.Themes.Classic,
    'categoryStyles': {
      'list_category': {
        'colour': "#4a148c"
      },
      'logic_category': {
        'colour': "#8b4513",
      },
      'loop_category': {
        'colour': "#85E21F",
      },
      'text_category': {
        'colour': "#FE9B13",
      },
    },
    'blockStyles': {
      'list_blocks': {
        'colourPrimary': "#4a148c",
        'colourSecondary': "#AD7BE9",
        'colourTertiary': "#CDB6E9",
      },
      'logic_blocks': {
        'colourPrimary': "#8b4513",
        'colourSecondary': "#ff0000",
        'colourTertiary': "#C5EAFF"
      },
      'loop_blocks': {
        'colourPrimary': '#FFAB19',
        'colourSecondary': '#FFAB19',
        'colourTertiary': '#8c5d0e',
      },
      'text_blocks': {
        'colourPrimary': "#FE9B13",
        'colourSecondary': "#ff0000",
        'colourTertiary': "#C5EAFF"
      },
      'drone_blocks': {
        'colourPrimary': "#FE9B13",
        'colourSecondary': "#ff0000",
        'colourTertiary': "#8c5d0e"
      },
    },
    'componentStyles': {
      'workspaceBackgroundColour': 'none',
      'toolboxBackgroundColour': 'none',
      'toolboxForegroundColour': 'none',
      'flyoutBackgroundColour': 'none',
      'flyoutForegroundColour': 'none',
      'flyoutOpacity': 1,
      'scrollbarColour': '#ff0000',
      'insertionMarkerColour': '#fff',
      'insertionMarkerOpacity': 0.3,
      'scrollbarOpacity': 0.4,
      'cursorColour': '#d0d0d0',
      'blackBackground': 'none'
    },
    'fontStyle':
    {
      "family": "Helvetica, sans-serif",
      "weight": "normal",
      "size": 12
    }
  });

}

export default SetTheme;