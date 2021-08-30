
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
window.testToolbox = `{
  "kind": "flyoutToolbox",
  "contents": [
    {
      "kind": "block",
      "type": "controls_repeat"
    },
    {
      "kind": "block",
      "type": "controls_whileUntil"
    },
    {
      "kind": "block",
      "type": "math_number"
    },
    {
      "kind": "block",
      "type": "math_single"
    },
    {
      "kind": "block",
      "type": "move_forward"
    }
    

  ]
}`

