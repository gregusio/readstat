# readstat

![dotnet workflow](https://github.com/gregusio/readstat/actions/workflows/dotnet.yml/badge.svg)


## Overview

Readstat is a comprehensive full-stack project designed for book lovers and readers, who want to get more insights into their reading statistics.

## Features

- Import books from Goodreads
- Add new books
- Delete books
- Update book information
- Create an account to save data
- View statistics (cool graphs)
- Posibility to draw next book to read

## Technologies

- React with typescript
- Material-UI
- .NET Core
- Entity Framework Core
- SQL Server

## Installation

### With Docker

1. Clone the repository

```bash
git clone https://github.com/gregusio/readstat.git
cd readstat
```

2. Run docker-compose

```bash
docker-compose up --build
```

3. Open the app in your browser - [http://localhost:3000/login](http://localhost:3000/login)

### Without Docker

1. Clone the repository

```bash
git clone
cd readstat
```

2. Install dependencies

```bash
npm install
cd frontend && npm install && cd ..
```

3. Run project

```bash
npm run start
```

4. Open the app in your browser - [http://localhost:3000/login](http://localhost:3000/login)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


