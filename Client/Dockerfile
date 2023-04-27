# Use an official Apache base image
FROM httpd:2.4

# Set the working directory to the Apache document root
WORKDIR /usr/local/apache2/htdocs

# Copy your web application files to the container
COPY . /usr/local/apache2/htdocs/Client

# Expose the default HTTP port
EXPOSE 80

# Start the Apache server when the container runs
CMD ["httpd-foreground"]
