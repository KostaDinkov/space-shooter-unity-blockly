import React, {useEffect} from "react";
import Blockly from 'blockly'
import './App.css';

function App() {
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
    let workspace = Blockly.inject(blocklyRef.current,{toolbox});
  });
  
  return (
    <div className="App">
      <div style={{height:"900px", width:"1000px"}} ref={blocklyRef}></div>
    </div>
  );
}

export default App;
