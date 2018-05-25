PS6-7 are the graphical implementations of PS1-6 in CS3500.1.
PS8-12 are subsequent iterations of our (user:mcnight and I) implementation of a Boggle game client and server. The server stores user and game data in successively more complex ways, and eventually runs exclusively on our own server structure (which, in the earler problem sets, runs on IIS Express).
    PS8 is a basic API-driven Boggle game client.
    PS9 is a very basic (and inefficient) version of the server that handles Boggle game data and responds to API requests.
    PS10 replaces PS9's data-storage static variables with a T-SQL database to contain game data (both past and current).
    PS11 creates a StringSocket on top of C#'s built-in TCP/IP Socket, which buffers strings instead of bytes.
    PS12 implements PS11's StringSocket, which (along with some minor new additions) replaces IIS Express to become its own independent service.

Per my instructor's (Joe Zachary) specifications, many of PS8-PS12's main directories are still "Spreadsheet", even though work on the Spreadsheet project conlcuded in PS7.
