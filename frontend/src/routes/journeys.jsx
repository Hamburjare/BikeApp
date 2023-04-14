import * as React from "react";
import { useState, useEffect } from "react";
import axios from "axios";
import { useTable } from "react-table";
import ReactPaginate from "react-paginate";

export default function App() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  let limit = 10;
  let orderBy = "id";
  let orderDir = "asc";
  let search = null;
  let maxDuration = 0;
  let minDuration = 0;
  let maxDistance = 0;
  let minDistance = 0;

  const columns = React.useMemo(
    () => [
      {
        Header: "Departure Station",
        accessor: "departureStationName",
      },
      {
        Header: "Return Station",
        accessor: "returnStationName",
      },
      {
        Header: "Covered Distance",
        accessor: (row) => `${(row.coveredDistance / 1000).toFixed(2)} km`,
      },
      {
        Header: "Duration",
        accessor: (row) => `${(row.duration / 60).toFixed(2)} min`,
      },
    ],
    []
  );

  useEffect(() => {
    const doFetch = async () => {
      await axios("https://localhost:5000/api/journeys")
        .then((res) => {
          setData(res.data.journeys);
          setPageCount(res.data.totalPages);
        })
        .catch((err) => setError(err))
        .finally(() => setIsLoaded(true));
    };
    doFetch();
  }, []);

  const tableInstance = useTable({ columns, data });

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    tableInstance;

  const handlePageClick = (event) => {
    handleMaxDistance(document.getElementById("maxDistance").value);
    handleMinDistance(document.getElementById("minDistance").value);
    handleMaxDuration(document.getElementById("maxDuration").value);
    handleMinDuration(document.getElementById("minDuration").value);
    handleLimit(document.getElementById("limit").value);
    search = document.getElementById("search").value;
    console.log(`User requested page number ${event.selected + 1}`);
    fetchJourneys(event.selected + 1);
  };

  const handleLimit = (event) => {
    limit = event;
  };

  const handleSearch = (event) => {
    handleMaxDistance(document.getElementById("maxDistance").value);
    handleMinDistance(document.getElementById("minDistance").value);
    handleMaxDuration(document.getElementById("maxDuration").value);
    handleMinDuration(document.getElementById("minDuration").value);
    handleLimit(document.getElementById("limit").value);
    search = event;
    setIsLoaded(false);
  };

  const handleOrderDir = (event) => {
    orderDir = event.target.value;
  };

  const handleOrderBy = (event) => {
    orderBy = event.target.value;
  };

  const handleMaxDuration = (event) => {
    maxDuration = event;
  };

  const handleMinDuration = (event) => {
    minDuration = event;
  };

  const handleMaxDistance = (event) => {
    maxDistance = event;
  };

  const handleMinDistance = (event) => {
    minDistance = event;
  };

  const handleReset = () => {
    document.getElementById("search").value = "";
    document.getElementById("orderDir").value = "asc";
    document.getElementById("orderBy").value = "id";
    document.getElementById("maxDuration").value = "";
    document.getElementById("minDuration").value = "";
    document.getElementById("maxDistance").value = "";
    document.getElementById("minDistance").value = "";
    document.getElementById("limit").value = 10;
    search = null;
    orderDir = "asc";
    orderBy = "id";
    maxDuration = 0;
    minDuration = 0;
    maxDistance = 0;
    minDistance = 0;
    limit = 10;
    setIsLoaded(false);
  };

  async function fetchJourneys(page) {
    let uri = `https://localhost:5000/api/journeys?page=${page}&limit=${limit}`;

    if (search && search.length) {
      // check length to avoid sending empty search string
      uri += `&search=${search}`;
    }

    if (orderDir) {
      uri += `&orderDir=${orderDir}`;
    } else {
      uri += `&orderDir=asc`;
    }

    if (orderBy) {
      uri += `&orderBy=${orderBy}`;
    }

    if (maxDuration) {
      uri += `&durationMax=${maxDuration}`;
    }

    if (minDuration) {
      uri += `&durationMin=${minDuration}`;
    }

    if (maxDistance) {
      uri += `&distanceMax=${maxDistance}`;
    }

    if (minDistance) {
      uri += `&distanceMin=${minDistance}`;
    }

    setData([]);
    await axios(uri)
      .then((res) => {
        setData(res.data.journeys);
        setPageCount(res.data.totalPages);
      })
      .catch((err) => setError(err))
      .finally(() => setIsLoaded(true));
  }

  const header = (
    <div className="mb-3">
      <div className="text-white text-center text-3xl mt-3">Journeys</div>
      <div className="flex justify-center">
        <div>
          <div className="text-white text-center text-xl mt-3">Search</div>
          <input
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white"
            type="text"
            id="search"
            name="search"
            placeholder="Search"
          />
        </div>
        <div>
          <div className="text-white text-center text-xl mt-3">Order By</div>
          <select
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white"
            id="orderBy"
            name="orderBy"
            onChange={handleOrderBy}
          >
            <option value="id">Default</option>
            <option value="departureStationName">Departure Station</option>
            <option value="returnStationName">Return Station</option>
            <option value="coveredDistance">Covered Distance</option>
            <option value="duration">Duration</option>
          </select>
        </div>
        <div>
          <div className="text-white text-center text-xl mt-3">Order Dir</div>
          <select
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white"
            id="orderDir"
            name="orderDir"
            onChange={handleOrderDir}
          >
            <option value="asc">Ascending</option>
            <option value="desc">Descending</option>
          </select>
        </div>
        <div>
          <div className="text-white text-center text-xl mt-3">Limit</div>
          <select
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white"
            id="limit"
            name="limit"
            onChange={handleLimit}
          >
            <option value="10">10</option>
            <option value="20">20</option>
            <option value="50">50</option>
            <option value="100">100</option>
          </select>
        </div>
      </div>

      <div className="flex justify-center">
        <div className="mr-2">
          <div className="text-white text-center text-md mt-3">
            Min Distance (m)
          </div>
          <input
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-32"
            type="number"
            id="minDistance"
            name="minDistance"
            placeholder="Number"
          />
        </div>
        <div className="mr-2">
          <div className="text-white text-center text-md mt-3">
            Max Distance (m)
          </div>
          <input
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-32"
            type="number"
            id="maxDistance"
            name="maxDistance"
            placeholder="Number"
          />
        </div>
        <div className="mr-2">
          <div className="text-white text-center text-md mt-3">
            Min Duration (s)
          </div>
          <input
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-32"
            type="number"
            id="minDuration"
            name="minDuration"
            placeholder="Number"
          />
        </div>
        <div className="mr-2">
          <div className="text-white text-center text-md mt-3">
            Max Duration (s)
          </div>
          <input
            className="bg-black border-solid border-2 border-white rounded p-2 mr-2 text-white w-32"
            type="number"
            id="maxDuration"
            name="maxDuration"
            placeholder="Number"
          />
        </div>

        <div className="mt-auto">
          <button
            className="border-2 border-white rounded p-2"
            onClick={() => {
              handleSearch(document.getElementById("search").value);
              fetchJourneys(1);
            }}
          >
            Search
          </button>
        </div>
        <div className="mt-auto">
          <button
            className="border-2 border-white rounded p-2 ml-2"
            onClick={() => {
              handleReset();
              fetchJourneys(1);
            }}
          >
            Reset
          </button>
        </div>
      </div>
    </div>
  );

  const body = React.useMemo(() => {
    return (
      <tbody {...getTableBodyProps()}>
        {rows.map((row) => {
          prepareRow(row);
          return (
            <tr {...row.getRowProps()}>
              {row.cells.map((cell) => {
                return (
                  <td
                    className="border-solid border-2 border-white px-2"
                    {...cell.getCellProps()}
                  >
                    {cell.render("Cell")}
                  </td>
                );
              })}
            </tr>
          );
        })}
      </tbody>
    );
  }, [getTableBodyProps, rows, prepareRow]);

  if (error) {
    return (
      <div>
        {header}
        <div className="text-white">Error: {error.message}</div>
      </div>
    );
  } else if (!isLoaded) {
    return (
      <div>
        {header}
        <div className="text-white">Loading...</div>
      </div>
    );
  } else {
    return (
      <div>
        {header}
        <div className="text-white border-solid border-2 border-white rounded">
          <table className="w-full" {...getTableProps()}>
            <thead>
              {headerGroups.map((headerGroup) => (
                <tr {...headerGroup.getHeaderGroupProps()}>
                  {headerGroup.headers.map((column) => (
                    <th
                      className="border-solid border-x-2 border-white px-2"
                      {...column.getHeaderProps()}
                    >
                      {column.render("Header")}
                    </th>
                  ))}
                </tr>
              ))}
            </thead>

            {body}
          </table>
          <footer className="text-center text-white mt-3">
            <ReactPaginate
              className="text-white flex justify-center"
              previousClassName="mx-2 mb-2"
              nextClassName="mx-2 mb-2"
              breakClassName="text-white font-bold py-2 px-4"
              pageClassName="mx-2 mb-2"
              breakLabel="..."
              nextLabel="next >"
              onPageChange={handlePageClick}
              pageRangeDisplayed={1}
              pageCount={pageCount}
              previousLabel="< previous"
              renderOnZeroPageCount={null}
              activeClassName="border-2 border-white rounded p-2"
            />
          </footer>
        </div>
      </div>
    );
  }
}
