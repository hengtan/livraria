# Base image for SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest

# Switch to root to install dependencies
USER root

# Update package list and install required tools
RUN apt-get update && apt-get install -y \
    curl \
    apt-transport-https \
    gnupg2 \
    unixodbc-dev

# Add Microsoft package signing key and SQL Server tools repository
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update

# Install ODBC Driver and SQLCMD tools
RUN ACCEPT_EULA=Y apt-get install -y \
    msodbcsql17 \
    mssql-tools

# Add SQLCMD tools to PATH
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

# Set default user back to mssql
USER mssql

# Expose SQL Server default port
EXPOSE 1433