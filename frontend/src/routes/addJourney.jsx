import axios from "axios";

export default function AddJourney() {
  function handleAdd() {
    const departureStationName = document.getElementById(
      "departureStationName"
    ).value;
    const returnStationName =
      document.getElementById("returnStationName").value;
    let departureStationId =
      document.getElementById("departureStationId").value;
    let returnStationId = document.getElementById("returnStationId").value;
    let departureTime = document.getElementById("departureTime").value;
    let returnTime = document.getElementById("returnTime").value;
    const coveredDistance = document.getElementById("coveredDistance").value;

    if (returnTime < departureTime) {
      alert("Return time must be after departure time");
      return;
    }

    const duration = Math.floor(
      (new Date(returnTime) - new Date(departureTime)) / 1000
    );

    if (
      !departureStationName ||
      !returnStationName ||
      !departureStationId ||
      !returnStationId ||
      !departureTime ||
      !returnTime ||
      !coveredDistance ||
      duration < 0 ||
      coveredDistance < 0 ||
      isNaN(departureStationId) ||
      isNaN(returnStationId) ||
      isNaN(coveredDistance) ||
      isNaN(duration)
    ) {
      alert("Please fill all fields correctly");
      return;
    }

    returnTime = new Date(returnTime).toISOString();
    departureTime = new Date(departureTime).toISOString();

    if (departureStationId.length < 3) {
      departureStationId =
        "0".repeat(3 - departureStationId.length) + departureStationId;
    }
    if (returnStationId.length < 3) {
      returnStationId =
        "0".repeat(3 - returnStationId.length) + returnStationId;
    }

    const journey = {
      departureTime: departureTime,
      returnTime: returnTime,
      departureStationId: departureStationId,
      departureStationName: departureStationName,
      returnStationId: returnStationId,
      returnStationName: returnStationName,
      coveredDistance: coveredDistance,
      duration: duration,
    };

    axios
      .post("https://localhost:5000/api/journeys", journey)
      .then((res) => {
        alert("Journey added. Redirecting to journeys page...");
        window.location.href = "/journeys";
      })
      .catch((err) => {
        alert(
          "Error adding journey. Please try again. If the problem persists, please contact the administrator.\n\n" +
            err
        );
      });
  }

  return (
    <div>
      <div className="text-white text-center text-3xl mt-3 mb-3">
        Add Journey
      </div>
      <div className="flex justify-center">
        <div className="w-1/2">
          <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="departureStationId"
              >
                Departure Station ID
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="departureStationId"
                type="number"
                required
                placeholder="Departure Station ID"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="departureStationName"
              >
                Departure Station Name
              </label>
              <div>
                <input
                  className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                  id="departureStationName"
                  type="text"
                  required
                  placeholder="Departure Station Name"
                />
              </div>
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="returnStationId"
              >
                Return Station ID
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="returnStationId"
                type="number"
                required
                placeholder="Return Station ID"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="returnStationName"
              >
                Return Station Name
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="returnStationName"
                type="text"
                required
                placeholder="Return Station Name"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="departureTime"
              >
                Departure Time
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="departureTime"
                type="datetime-local"
                required
                pattern="[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}"
                placeholder="Departure Time"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="returnTime"
              >
                Return Time
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="returnTime"
                type="datetime-local"
                required
                pattern="[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}"
                placeholder="Return Time"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="coveredDistance"
              >
                Covered Distance (m)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="coveredDistance"
                type="number"
                required
                placeholder="Covered Distance (m)"
              />
            </div>

            <div className="flex items-center justify-between">
              <button
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                type="button"
                onClick={() => {
                  handleAdd();
                }}
              >
                Add Journey
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
