import React from 'react';
import Blockly from 'blockly';

export default function BlocklyInit(){

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

  Blockly.JavaScript['move_forward'] = function(block) {
    var code = 'await move_forward();\n';
    return code;
  };
  
  Blockly.JavaScript['rotate_left'] = function(block) {

    var code = 'await rotate_left();\n';
    return code;
  };
  
  Blockly.JavaScript['rotate_right'] = function(block) {
    
    var code = 'await rotate_right();\n';
    return code;
  };
}