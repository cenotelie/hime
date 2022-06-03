FROM buildpack-deps:22.04-curl
LABEL maintainer="Laurent Wouters <lwouters@cenotelie.fr>" vendor="Association Cénotélie" description="Hime Build environment"

# Custom Home
RUN mkdir /home/builder
ENV HOME=/home/builder

# Add required packages
RUN apt update && apt install -y --no-install-recommends \
		build-essential \
		git \
		libssl-dev \
		musl-tools \
    && rm -rf /var/lib/apt/lists/*

# Add support for Rust
RUN curl https://sh.rustup.rs -sSf | sh -s -- -y && \
	export PATH="${HOME}/.cargo/bin:${PATH}" && \
    rustup target add x86_64-unknown-linux-musl && \
	mkdir /home/builder/.cargo/registry && \
	chmod -R a+rwX /home/builder

# Environment
ENV PATH="${HOME}/.cargo/bin:${PATH}"
ENV RUST_BACKTRACE=1
ENV HOME=/home/builder

# Specify volumes
VOLUME /src
VOLUME /home/builder/.cargo/registry