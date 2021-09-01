
import toolbox from "./toolbox.js";
import InitCustomBlocks from './custom_blocks.js';
import SetTheme from './theme.js';


InitCustomBlocks();
SetTheme();

let workspace;

function SetWorkSpace(toolbox) {

  let blocklyDiv = document.getElementById("blocklyDiv");
  blocklyDiv.innerHTML = "";
  let toolboxJson = JSON.parse(toolbox);
  console.log(toolboxJson);
  workspace = Blockly.inject('blocklyDiv', {
    toolbox:toolboxJson,
    scrollbars: false,
    renderer: "zelos",
    theme: "kiberlab"
  });

}
function getCode() {
  return Blockly.CSharp.workspaceToCode(workspace);
}

window.getCode = getCode;
window.setWorkSpace = SetWorkSpace;
