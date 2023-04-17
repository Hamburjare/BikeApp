import axios from "axios";

export default function AddStation() {
  function handleAdd() {
    let stationId = document.getElementById("stationId").value;
    const nameFin = document.getElementById("nameFin").value;
    const nameEng = document.getElementById("nameEng").value;
    const nameSwe = document.getElementById("nameSwe").value;
    const cityFin = document.getElementById("cityFin").value;
    const citySwe = document.getElementById("citySwe").value;
    const addressFin = document.getElementById("addressFin").value;
    const addressSwe = document.getElementById("addressSwe").value;
    const operator = document.getElementById("operator").value;
    const capacity = document.getElementById("capacity").value;
    let latitude = document.getElementById("latitude").value;
    let longitude = document.getElementById("longitude").value;

    if (stationId.length < 3) {
      stationId = "0".repeat(3 - stationId.length) + stationId;
    }

    latitude = latitude.replace(",", ".");
    longitude = longitude.replace(",", ".");

    if (
      !stationId ||
      !nameFin ||
      !nameEng ||
      !nameSwe ||
      !cityFin ||
      !citySwe ||
      !addressFin ||
      !addressSwe ||
      !operator ||
      !capacity ||
      !latitude ||
      !longitude ||
      isNaN(stationId) ||
      isNaN(capacity) ||
      isNaN(latitude) ||
      isNaN(longitude) ||
      latitude < -90 ||
      latitude > 90 ||
      longitude < -180 ||
      longitude > 180
    ) {
      alert("Please fill all fields correctly");
      return;
    }

    const station = {
      id: stationId,
      nameFIN: nameFin,
      nameSWE: nameSwe,
      nameENG: nameEng,
      addressFIN: addressFin,
      addressSWE: addressSwe,
      cityFIN: cityFin,
      citySWE: citySwe,
      operator: operator,
      capacity: capacity,
      latitude: latitude,
      longitude: longitude,
    };

    axios
      .post("https://backend.hamburjare.tech/api/stations", station)
      .then((res) => {
        alert("Station added successfully, redirecting to stations page...");
        window.location.href = "/stations";
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
        Add Station
      </div>
      <div className="flex justify-center">
        <div className="w-1/2">
          <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="stationId"
              >
                Station ID
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="stationId"
                type="number"
                required
                placeholder="Station ID"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="nameFin"
              >
                Station Name (Finnish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="nameFin"
                type="text"
                required
                placeholder="Station Name (Finnish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="nameEng"
              >
                Station Name (English)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="nameEng"
                type="text"
                required
                placeholder="Station Name (English)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="nameSwe"
              >
                Station Name (Swedish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="nameSwe"
                type="text"
                required
                placeholder="Station Name (Swedish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="addressFin"
              >
                Address (Finnish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="addressFin"
                type="text"
                required
                placeholder="Address (Finnish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="addressSwe"
              >
                Address (Swedish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="addressSwe"
                type="text"
                required
                placeholder="Address (Swedish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="cityFin"
              >
                City (Finnish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="cityFin"
                type="text"
                required
                placeholder="City (Finnish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="citySwe"
              >
                City (Swedish)
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="citySwe"
                type="text"
                required
                placeholder="City (Swedish)"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="operator"
              >
                Operator
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="operator"
                type="text"
                required
                placeholder="Operator"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="capacity"
              >
                Capacity
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="capacity"
                type="number"
                required
                placeholder="Capacity"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="latitude"
              >
                Latitude
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="latitude"
                type="text"
                required
                placeholder="Latitude"
              />
            </div>
            <div className="mb-4">
              <label
                className="block text-gray-700 text-sm font-bold mb-2"
                htmlFor="longitude"
              >
                Longitude
              </label>
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline invalid:border-red-500"
                id="longitude"
                type="text"
                required
                placeholder="Longitude"
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
                Add Station
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
