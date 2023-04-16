import * as React from "react";
import { useState, useEffect } from "react";
import axios from "axios";
import { useTable } from "react-table";
import { Link } from "react-router-dom";
import ReactPaginate from "react-paginate";

export default function App() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  let limit = 10;
  let search = null;

  const columns = React.useMemo(
    () => [
      {
        Header: "ID",
        accessor: "id",
      },
      {
        Header: "Name (FIN)",
        accessor: "nameFIN",
      },
      {
        Header: "Name (SWE)",
        accessor: "nameSWE",
      },
      {
        Header: "Address (FIN)",
        accessor: "addressFIN",
      },
      {
        Header: "Address (SWE)",
        accessor: "addressSWE",
      },
      {
        Header: "City (FIN)",
        accessor: "cityFIN",
      },
      {
        Header: "City (SWE)",
        accessor: "citySWE",
      },
      {
        Header: "Operator",
        accessor: "operator",
      },
      {
        Header: "Capacity",
        accessor: "capacity",
      },
      {
        Header: "Latitude",
        accessor: "latitude",
      },
      {
        Header: "Longitude",
        accessor: "longitude",
      },
    ],
    []
  );

  useEffect(() => {
    const doFetch = async () => {
      await axios("https://localhost:5000/api/stations")
        .then((res) => {
          setData(res.data.stations);
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
    handleLimit(document.getElementById("limit").value);
    search = document.getElementById("search").value;
    console.log(`User requested page number ${event.selected + 1}`);
    fetchData(event.selected + 1);
  };

  const handleLimit = (event) => {
    limit = event;
  };

  const handleSearch = (event) => {
    handleLimit(document.getElementById("limit").value);
    search = event;
    setIsLoaded(false);
  };

  const handleReset = () => {
    document.getElementById("search").value = "";
    document.getElementById("limit").value = 10;
    search = null;
    limit = 10;
    setIsLoaded(false);
  };

  async function fetchData(page) {
    let uri = `https://localhost:5000/api/stations?page=${page}&limit=${limit}`;

    if (search && search.length) {
      // check length to avoid sending empty search string
      uri += `&search=${search}`;
    }

    setData([]);
    await axios(uri)
      .then((res) => {
        setData(res.data.stations);
        setPageCount(res.data.totalPages);
      })
      .catch((err) => setError(err))
      .finally(() => setIsLoaded(true));
  }

  const header = (
    <div className="mb-3">
      <div className="text-white text-center text-3xl mt-3">Stations</div>
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

        <div className="mt-auto">
          <button
            className="border-2 border-white rounded p-2"
            onClick={() => {
              handleSearch(document.getElementById("search").value);
              fetchData(1);
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
              fetchData(1);
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
              <td className="border-solid border-2 border-white px-2">
                <Link
                  to = {`/stations/${row.original.id}`}
                >
                  <button className="border-2 border-white rounded p-2">
                    View
                  </button>
                </Link>

              </td>
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
          <table className="w-fit" {...getTableProps()}>
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
