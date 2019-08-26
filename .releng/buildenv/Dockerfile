FROM buildpack-deps:19.04-curl
LABEL maintainer="Laurent Wouters <lwouters@cenotelie.fr>" vendor="Association Cénotélie" description="Hime Build environment"

# Custom Home
RUN mkdir /home/builder
ENV HOME=/home/builder

# Add required packages
RUN apt update && apt-get dist-upgrade -y && apt install -y --no-install-recommends \
		software-properties-common \
		apt-transport-https \
		gnupg \
		ca-certificates \
		build-essential \
		zip \
		git \
		pkg-config \
    && rm -rf /var/lib/apt/lists/*

# Add dotnet core and Mono
RUN wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
	&& dpkg -i packages-microsoft-prod.deb \
	&& rm packages-microsoft-prod.deb
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | tee /etc/apt/sources.list.d/mono-official-stable.list
RUN apt update && apt-get install -y --no-install-recommends \
		dotnet-sdk-2.2 \
		mono-devel \
    && rm -rf /var/lib/apt/lists/*

# Add JDK 8 and maven
RUN add-apt-repository ppa:openjdk-r/ppa && apt install -y --no-install-recommends \
		openjdk-8-jdk \
		maven \
    && rm -rf /var/lib/apt/lists/*

# Add support for Rust
RUN curl https://sh.rustup.rs -sSf | sh -s -- -y
ENV PATH="${HOME}/.cargo/bin:${PATH}"
RUN rustup install nightly
RUN rustup component add rustfmt-preview --toolchain nightly
RUN rustup component add clippy
RUN mkdir /home/builder/.cargo/registry
ENV RUST_BACKTRACE=1

# Setup ownership/permissions
RUN chmod -R a+rwX /home/builder

# Specify volumes
VOLUME /src
VOLUME /home/builder/.cargo/registry