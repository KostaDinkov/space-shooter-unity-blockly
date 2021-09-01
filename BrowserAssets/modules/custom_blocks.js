export default function InitCustomBlocks(){

  Blockly.defineBlocksWithJsonArray([
  {
    "type": "move_forward",
    "message0": "Премести Напред",
    "previousStatement": null,
    "nextStatement": null,
    "style": "drone_blocks",
    "tooltip": "Премества дрона 1 поле напред",
    "helpUrl": ""
  },
  {
    "type": "rotate_left",
    "message0": "Завърти Наляво",
    "previousStatement": null,
    "nextStatement": null,
    "style": "drone_blocks",
    "tooltip": "Завърта дрона 90 градуса наляво",
    "helpUrl": ""
  },
  {
    "type": "rotate_right",
    "message0": "Завърти Надясно",
    "previousStatement": null,
    "nextStatement": null,
    "style": "drone_blocks",
    "tooltip": "Завърта дрона 90 градуса надясно",
    "helpUrl": ""
  },
  {
    "type": "fire_weapon",
    "message0": "Стреляй",
    "previousStatement": null,
    "nextStatement": null,
    "style": "drone_blocks",
    "tooltip": "Стреля с лазер",
    "helpUrl": ""
  },
  {
    "type": "scan_ahead",
    "message0": "Сканирай Напред",
    "output": "String",
    "style": "drone_blocks",
    "tooltip": "Сканира обекта директно пред дрона",
    "helpUrl": ""
  },
  {
    "type": "pickup_object",
    "message0": "Натовари Отпред",
    "previousStatement": null,
    "nextStatement": null,
    "style": "drone_blocks",
    "tooltip": "Товари обекта директно пред дрона(ако обекта може да бъде товарен)",
    "helpUrl": ""
  },
  

])

  
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

  Blockly.CSharp['scan_ahead'] = function(block) {
    var code = 'await Player.ScanAheadAsync();';
    return [code, Blockly.CSharp.ORDER_NONE];
  };

  Blockly.CSharp['fire_weapon'] = function(block) {
    var code = 'await Player.FireWeaponAsync();';
    return code;
  };
  
  Blockly.CSharp['pickup_object'] = function(block) {
    var code = 'await Player.PickupObjectAsync();';
    return code;
  };
}