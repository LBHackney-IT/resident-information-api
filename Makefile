.PHONY: setup
setup:
	docker-compose build

.PHONY: build
build:
	docker-compose build base-api

.PHONY: serve
serve:
	docker-compose build resident-information-api && docker-compose up resident-information-api

.PHONY: shell
shell:
	docker-compose run resident-information-api bash

.PHONY: test
test:
	docker-compose up test-database & docker-compose build resident-information-api-test && docker-compose up resident-information-api-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format
