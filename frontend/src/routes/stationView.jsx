import { useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import Loading from "../Loading";

export default function StationView() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [isUpdating, setIsUpdating] = useState(false);
  const [stationData, setStationData] = useState([]);
  const [departureJourneys, setDepartureJourneys] = useState(0);
  const [returnJourneys, setReturnJourneys] = useState(0);
  const [avgDepartureDistance, setAvgDepartureDistance] = useState(0);
  const [avgReturnDistance, setAvgReturnDistance] = useState(0);
  const [top5DepartureStations, setTop5DepartureStations] = useState([]);
  const [top5ReturnStations, setTop5ReturnStations] = useState([]);

  const location = useLocation();

  const pathName = location.pathname;

  const id = pathName.split("/")[2];

  const handleMonthChange = (e) => {
    setIsUpdating(true);
    const months = [
      "january",
      "february",
      "march",
      "april",
      "may",
      "june",
      "july",
      "august",
      "september",
      "october",
      "november",
      "december",
    ];

    let uri = `${import.meta.env.VITE_API_URL}/stations/${id}`;

    if (months.includes(e.target.value.toLowerCase())) {
      uri += `?month=${e.target.value}`;
    }

    const doFetch = async () => {
      await axios(uri)
        .then((res) => {
          res.data.station.longitude = res.data.station.longitude.replace(
            ",",
            "."
          );
          res.data.station.latitude = res.data.station.latitude.replace(
            ",",
            "."
          );
          setStationData(res.data.station);
          setDepartureJourneys(res.data.departureJourneys);
          setReturnJourneys(res.data.returnJourneys);
          setAvgDepartureDistance(res.data.avarageDepartureDistance);
          setAvgReturnDistance(res.data.avarageReturnDistance);
          setTop5DepartureStations(res.data.top5DepartureStations);
          setTop5ReturnStations(res.data.top5ReturnStations);
        })
        .catch((err) => setError(err))
        .finally(() => setIsUpdating(false));
    };
    doFetch();
  };

  useEffect(() => {
    const doFetch = async () => {
      await axios(`${import.meta.env.VITE_API_URL}/stations/${id}`)
        .then((res) => {
          res.data.station.longitude = res.data.station.longitude.replace(
            ",",
            "."
          );
          res.data.station.latitude = res.data.station.latitude.replace(
            ",",
            "."
          );
          setStationData(res.data.station);
          setDepartureJourneys(res.data.departureJourneys);
          setReturnJourneys(res.data.returnJourneys);
          setAvgDepartureDistance(res.data.avarageDepartureDistance);
          setAvgReturnDistance(res.data.avarageReturnDistance);
          setTop5DepartureStations(res.data.top5DepartureStations);
          setTop5ReturnStations(res.data.top5ReturnStations);
        })
        .catch((err) => setError(err))
        .finally(() => setIsLoaded(true));
    };
    doFetch();
  }, []);

  function Stats() {
    if (isUpdating) {
      return <Loading />;
    }
    return (
      <div className="flex flex-col justify-center items-center">
        <div className="text-white text-md mt-3">
          Total number of journeys starting from this station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {departureJourneys}
        </div>
        <div className="text-white text-md mt-3">
          Total number of journeys ending at this station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {returnJourneys}
        </div>
        <div className="text-white text-md mt-3">
          The average distance of a journey starting from this station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {(avgDepartureDistance / 1000).toFixed(2)} km
        </div>
        <div className="text-white text-md mt-3">
          The average distance of a journey ending at this station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {(avgReturnDistance / 1000).toFixed(2)} km
        </div>
        <div className="text-white text-md mt-3">
          Top 5 most popular departure stations for journeys ending at this
          station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {top5DepartureStations.map((station, index) => (
            <div>
              {index + 1}. {station}
            </div>
          ))}
        </div>
        <div className="text-white text-md mt-3">
          Top 5 most popular return stations for journeys starting from this
          station
        </div>
        <div className="text-white text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit">
          {top5ReturnStations.map((station, index) => (
            <div>
              {index + 1}. {station}
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <Loading />;
  } else {
    return (
      <div className="w-fit mx-auto">
        <h1 className="text-center text-3xl mb-2 text-white">Station view</h1>
        <div className="flex justify-center">
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">ID</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.id}
            </div>
          </div>
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">Name</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.nameFIN}
            </div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.nameSWE}
            </div>
          </div>
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">Address</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.addressFIN}
            </div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.addressSWE}
            </div>
          </div>
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">City</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.cityFIN}
            </div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.citySWE}
            </div>
          </div>
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">Operator</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.operator}
            </div>
          </div>
          <div className="flex flex-col mr-3">
            <div className="text-white text-center text-md mt-3">Capacity</div>
            <div className="text-white text-center text-md mt-3 bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white">
              {stationData.capacity}
            </div>
          </div>
        </div>
        <div>
          <h1 className="text-center text-3xl mb-2 mt-10 text-white">Stats</h1>
          <div className="flex justify-center flex-col mr-3 text-center items-center">
            <div className="text-white text-xl mt-3">Filter by month</div>
            <select
              className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-fit"
              id="filter"
              name="filter"
              onChange={handleMonthChange}
            >
              <option value="all">All</option>
              <option value="january">January</option>
              <option value="february">February</option>
              <option value="march">March</option>
              <option value="april">April</option>
              <option value="may">May</option>
              <option value="june">June</option>
              <option value="july">July</option>
              <option value="august">August</option>
              <option value="september">September</option>
              <option value="october">October</option>
              <option value="november">November</option>
              <option value="december">December</option>
            </select>
            <Stats />
          </div>
        </div>

        <MapContainer
          className="mt-3 mb-11 h-96 w-full"
          center={[stationData.latitude, stationData.longitude]}
          zoom={12}
          scrollWheelZoom={false}
        >
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
          <Marker position={[stationData.latitude, stationData.longitude]}>
            <Popup>
              Name: {stationData.nameFIN}, {stationData.nameSWE} <br />
              Address: {stationData.addressFIN}, {stationData.addressSWE} <br />
              City: {stationData.cityFIN}, {stationData.citySWE} <br />
              Operator: {stationData.operator} <br />
              Capacity: {stationData.capacity} <br />
            </Popup>
          </Marker>
        </MapContainer>
      </div>
    );
  }
}
