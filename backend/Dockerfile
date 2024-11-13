FROM golang:1.23.1 AS build
WORKDIR /app
COPY go.mod ./
RUN go mod download
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o /backend-app
EXPOSE 8080
CMD ["/backend-app"]