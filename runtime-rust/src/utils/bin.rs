/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 ******************************************************************************/

//! Module for binary manipulation APIs

/// reads a u16 from an array of bytes
pub fn read_u16(buffer: &[u8], index: usize) -> u16 {
    ((buffer[index + 1] as u16) << 8 | (buffer[index] as u16))
}

/// reads a u32 from an array of bytes
pub fn read_u32(buffer: &[u8], index: usize) -> u32 {
    ((buffer[index + 3] as u32) << 24
        | (buffer[index + 2] as u32) << 16
        | (buffer[index + 1] as u32) << 8
        | (buffer[index] as u32))
}

/// Reads a table of u16 from a byte buffer
pub fn read_table_u16(buffer: &[u8], start: usize, count: usize) -> Vec<u16> {
    let mut result = Vec::<u16>::with_capacity(count);
    for i in 0..count {
        result.push(read_u16(buffer, start + i * 2));
    }
    result
}

/// Reads a table of u32 from a byte buffer
pub fn read_table_u32(buffer: &[u8], start: usize, count: usize) -> Vec<u32> {
    let mut result = Vec::<u32>::with_capacity(count);
    for i in 0..count {
        result.push(read_u32(buffer, start + i * 4));
    }
    result
}
