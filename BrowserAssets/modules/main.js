
import toolbox from "./toolbox.js";
import InitCustomBlocks from './custom_blocks.js';


InitCustomBlocks();

let workspace = Blockly.inject('blocklyDiv', {
  toolbox,
  scrollbars: false
});

function getCode(){
  return Blockly.CSharp.workspaceToCode(workspace);
}

window.getCode = getCode;

