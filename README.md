# neo-simulation

A model of all potentially hazardous NEOs based on their Keplerian elements.  
The aim of the project is to visualize all of the currently known PHAs and 
provide information for each individual visualized object on demand.

---

## Data
The data used in this project is from [JPL's Small-Body Database](https://ssd.jpl.nasa.gov/tools/sbdb_query.html). The following are
the limits and output fields used to query the database:
1. Limit by Object Group: `PHAs`,  
   Limit by Object Kind: `asteroids`.
2. Output Fields:
   -  `object fullname`
   -  `epoch.mjd`
   -  `a`
   -  `e`
   -  `i`
   -  `node`
   -  `peri`
   -  `M`
   -  `n`
   -  `Earth MOID`
   -  `diameter`
    
The current data used in the project is valid as of _24.09.2021_.

## References
1. NEO orbit data: _JPL's Small-Body Database (https://ssd.jpl.nasa.gov/tools/sbdb_query.html)_.
2. Large object (planets) orbit data: _Approximate Positions of the Planets, Table 2a (https://ssd.jpl.nasa.gov/planets/approx_pos.html)_.
3. Large object (planets) diameters: _List of Solar System objects by size on Wikipedia (https://en.wikipedia.org/wiki/List_of_Solar_System_objects_by_size#Larger_than_400_km)_.
4. The algorithm used in [Computation.PositionAtEccentricAnomaly](https://github.com/gzhynko/neo-simulation/blob/master/NEOSimulation/Utils/Computation.cs#31): _2012rcampion on StackExchange Space (https://space.stackexchange.com/a/8915)_.
