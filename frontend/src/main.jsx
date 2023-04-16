import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./index.css";
import Root from "./routes/root.jsx";
import ErrorPage from "./error.jsx";
import Journeys from "./routes/journeys.jsx";
import Stations from "./routes/stations.jsx";
import StationView from "./routes/stationView.jsx";
import AddJourney from "./routes/addJourney.jsx";
import AddStation from "./routes/addStation";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "journeys",
        element: <Journeys />,
        errorElement: <ErrorPage />,
      },
      {
        path: "journeys/add",
        element: <AddJourney />,
        errorElement: <ErrorPage />,
      },
      {
        path: "stations",
        element: <Stations />,
        errorElement: <ErrorPage />,
      },
      {
        path: "stations/:id",
        element: <StationView />,
        errorElement: <ErrorPage />,
      },
      {
        path: "stations/add",
        element: <AddStation />,
        errorElement: <ErrorPage />,
      }
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
      <RouterProvider router={router} />
  </React.StrictMode>
);
