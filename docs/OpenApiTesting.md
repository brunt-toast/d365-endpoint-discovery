# OpenAPI Testing 

To test the OpenAPI command's output, you can use the swaggerapi/swagger-ui docker container: 

```bash
docker run -p 8080:8080 \
  -e SWAGGER_JSON=/foo/swagger.json \
  -v "$(pwd):/foo" \
  swaggerapi/swagger-ui
```
