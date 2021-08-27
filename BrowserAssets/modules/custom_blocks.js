export default function InitCustomBlocks(){

  Blockly.defineBlocksWithJsonArray([{
    "type": "move_forward",
    "message0": "Премести Напред",
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "Премества дрона 1 поле напред",
    "helpUrl": ""
  },
  {
    "type": "rotate_left",
    "message0": "Завърти Наляво",
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "Завърта дрона 90 градуса наляво",
    "helpUrl": ""
  },
  {
    "type": "rotate_right",
    "message0": "Завърти Надясно",
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "Завърта дрона 90 градуса надясно",
    "helpUrl": ""
  }])

  
  Blockly.CSharp['move_forward'] = function(block) {
    var code = 'await Player.MoveForwardAsync();\n';
    return code;
  };
  
  Blockly.CSharp['rotate_left'] = function(block) {

    var code = 'await Player.RotateLeftAsync();\n';
    return code;
  };
  
  Blockly.CSharp['rotate_right'] = function(block) {
    
    var code = 'await Player.RotateRightAsync();\n';
    return code;
  };
}