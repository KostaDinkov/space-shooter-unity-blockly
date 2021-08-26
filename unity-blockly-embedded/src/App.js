import React, {useEffect, useState} from "react";
import Blockly from 'blockly'
import './App.css';
import BlocklyInit from "./blockly/blockly_init";
import javascript from "blockly/javascript";

function App() {
  BlocklyInit();
  let [workspace, setWorkspace] = useState({});
  let blocklyRef = React.createRef();
  let toolbox = {
    "kind": "categoryToolbox",
    "contents": [
      {
        "kind": "category",
        "name": "Control",
        "contents": [
          {
            "kind": "block",
            "type": "controls_if"
          },
          {
            "kind": "block",
            "type": "controls_whileUntil"
          },
          {
            "kind": "block",
            "type": "controls_for"
          },
          {
            "kind":"block",
            "type":"move_forward"
          }
        ]
      },
      {
        "kind": "category",
        "name": "Logic",
        "contents": [
          {
            "kind": "block",
            "type": "logic_compare"
          },
          {
            "kind": "block",
            "type": "logic_operation"
          },
          {
            "kind": "block",
            "type": "logic_boolean"
          }
        ]
      }
    ]
  }

  
  
  useEffect(()=>{
    setWorkspace(Blockly.inject(blocklyRef.current,{toolbox}));
  },[]);

  
  

  const runCode = ()=>{
    let code =  javascript.workspaceToCode(workspace); 
    let template = "async function ExecuteCodeAsync() {\n" + code + "\n}\nExecuteCodeAsync();blockly"
    console.log(template);
    eval(template);
  }
  
  return (
    <div className="App">
      <button onClick ={runCode}>Run Code</button>
      <div style={{height:"900px", width:"1000px"}} ref={blocklyRef}></div>
    </div>
  );
}

export default App;
