

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
    toolbox: toolboxJson,
    scrollbars: true,
    renderer: "zelos",
    theme: "kiberlab",
    collapse: false,
    trashcan: false,
    zoom:
    {
      controls: true,
      wheel: true,
      startScale: 1.0,
      maxScale: 3,
      minScale: 0.3,
      scaleSpeed: 1.2,
      pinch: false
    },
    grid:
    {
      spacing: 50,
      length: 10,
      colour: '#49F5ED55',
      snap: true
    },
  });
  workspace.toolbox_.flyout_.autoClose = false;




}
function getCode() {
  return Blockly.CSharp.workspaceToCode(workspace);
}

window.getCode = getCode;
window.setWorkSpace = SetWorkSpace;
