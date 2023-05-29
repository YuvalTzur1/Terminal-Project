import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/terminalhub") 
  .build();

const signalRService = {
  startConnection: async () => {
    try {
      if (connection.state === signalR.HubConnectionState.Disconnected) {
        await connection.start();
        console.log("Connected to SignalR hub");
      } else {
        console.log("SignalR connection is already in progress or Server not running.");
      }
    } catch (error) {
      console.error("Error connecting to SignalR hub:", error);
    }
  },

  addFlightReceivedListener: (callback) => {
    connection.on("flightReceived", callback);
  },

  addFlightRemoveListener: (callback) => {
    connection.on("flightRemove", callback);
  },

  addFlightMoveListener: (callback) => {
    connection.on("flightMoove", callback);
  }
};

export default signalRService;