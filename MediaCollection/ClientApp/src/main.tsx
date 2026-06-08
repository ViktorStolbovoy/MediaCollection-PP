import React from 'react';
import ReactDOM from 'react-dom/client';
import { HashRouter } from 'react-router-dom';
import { App } from './App';
import { WebSocketProvider } from './WebSocketProvider';
import './index.css';

const rootEl = document.getElementById('root');
if (!rootEl) throw new Error('#root missing');

ReactDOM.createRoot(rootEl).render(
  <React.StrictMode>
    <HashRouter>
      <WebSocketProvider>
        <App />
      </WebSocketProvider>
    </HashRouter>
  </React.StrictMode>
);
